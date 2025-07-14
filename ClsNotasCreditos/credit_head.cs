using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ControlOPC.Entidades;

namespace ClsNotasCreditos
{
    public class credit_head : Base
    {
        #region "Variables"

        private Int64 _nc_id;
        private DateTime? _nc_date = null;
        private string _nc_concept = string.Empty;
        private string _nc_number = string.Empty;
        private string _nc_authorization = string.Empty;
        private int  _id_concept;
        private bool _nc_state;
        private decimal _nc_subtotal = 0;
	    private decimal _nc_iva = 0;
        private decimal _nc_total = 0;
        private decimal _nc_valor_nivel = 0;
        private Int64 _id_level;
        private Int64 _id_factura;
	    private string _num_factura = string.Empty;
        private DateTime? _fec_factura = null;

        private Int64 _id_cliente;
	    private string _ruc_cliente = string.Empty;
        private string _nombre_cliente = string.Empty;
        private string _email_cliente = string.Empty;
        private decimal _total_factura = 0;
        private decimal _iva_factura = 0;
        private decimal _porc_iva = 0;
        private string _description = string.Empty;
        private string _nc_date_text = string.Empty;
        private string _level_text = string.Empty;
        private string _usuarios_aprobados = string.Empty;
        private string _usuarios_pendientes = string.Empty;
        private string _nc_anulacion = string.Empty;
        private bool _DetalleLoaded;
        private string _xmlDocumentos = string.Empty;
        private string _ruta_documento = string.Empty;
        private Int64 _id_group;
        private int _IdUsuario;
        private int _level;

        private string _nc_xml_draft = string.Empty;
        private string _nc_xml_final = string.Empty;
        private string _tipo = string.Empty;
        private string _direccion_archivo = string.Empty;
        private string _mensaje_archivo = string.Empty;
        private string _usuario = string.Empty;
        private string _descripcion = string.Empty;

        #endregion

        #region "Propiedades"

        public Int64 nc_id { get => _nc_id; set => _nc_id = value; }
        public DateTime? nc_date { get => _nc_date; set => _nc_date = value; }
        public string nc_concept { get => _nc_concept; set => _nc_concept = value; }
        public string nc_number { get => _nc_number; set => _nc_number = value; }
        public string nc_authorization { get => _nc_authorization; set => _nc_authorization = value; }
        public int id_concept { get => _id_concept; set => _id_concept = value; }
        public bool nc_state { get => _nc_state; set => _nc_state = value; }
        public decimal nc_subtotal { get => _nc_subtotal; set => _nc_subtotal = value; }
        public decimal nc_iva { get => _nc_iva; set => _nc_iva = value; }
        public decimal nc_total { get => _nc_total; set => _nc_total = value; }
        public decimal nc_valor_nivel { get => _nc_valor_nivel; set => _nc_valor_nivel = value; }
        public Int64 id_level { get => _id_level; set => _id_level = value; }
        public Int64 id_factura { get => _id_factura; set => _id_factura = value; }
        public string num_factura { get => _num_factura; set => _num_factura = value; }
        public DateTime? fec_factura { get => _fec_factura; set => _fec_factura = value; }

        public Int64 id_cliente { get => _id_cliente; set => _id_cliente = value; }
        public string ruc_cliente { get => _ruc_cliente; set => _ruc_cliente = value; }
        public string nombre_cliente { get => _nombre_cliente; set => _nombre_cliente = value; }
        public string email_cliente { get => _email_cliente; set => _email_cliente = value; }
        public decimal total_factura { get => _total_factura; set => _total_factura = value; }
        public decimal iva_factura { get => _iva_factura; set => _iva_factura = value; }
        public decimal porc_iva { get => _porc_iva; set => _porc_iva = value; }

        public bool IsDetalleLoaded { get { return _DetalleLoaded; } }
        public string description { get => _description; set => _description = value; }

        public string nc_date_text { get => _nc_date_text; set => _nc_date_text = value; }
        public string level_text { get => _level_text; set => _level_text = value; }
        public string usuarios_aprobados { get => _usuarios_aprobados; set => _usuarios_aprobados = value; }
        public string usuarios_pendientes { get => _usuarios_pendientes; set => _usuarios_pendientes = value; }
        public string nc_anulacion { get => _nc_anulacion; set => _nc_anulacion = value; }
        public string xmlDocumentos { get => _xmlDocumentos; set => _xmlDocumentos = value; }
        public string ruta_documento { get => _ruta_documento; set => _ruta_documento = value; }
        public string nc_xml_draft { get => _nc_xml_draft; set => _nc_xml_draft = value; }
        public string nc_xml_final { get => _nc_xml_final; set => _nc_xml_final = value; }
        public string tipo { get => _tipo; set => _tipo = value; }
       
        private static String v_mensaje = string.Empty;

        public Int64 id_group { get => _id_group; set => _id_group = value; }
        public int IdUsuario { get => _IdUsuario; set => _IdUsuario = value; }
        public int level { get => _level; set => _level = value; }

        public string direccion_archivo { get => _direccion_archivo; set => _direccion_archivo = value; }
        public string mensaje_archivo { get => _mensaje_archivo; set => _mensaje_archivo = value; }
        public string usuario { get => _usuario; set => _usuario = value; }
        public string descripcion { get => _descripcion; set => _descripcion = value; }

        #endregion

        public List<credit_detail> Detalle { get; set; }
        public List<credit_level_approval> DetalleNivel { get; set; }

        public credit_head(Int64 _nc_id, DateTime? _nc_date,string _nc_concept,string _nc_number,string _nc_authorization ,int _id_concept,
        bool _nc_state,decimal _nc_subtotal,decimal _nc_iva,decimal _nc_total,decimal _nc_valor_nivel ,Int64 _id_level,Int64 _id_factura,
        string _num_factura,DateTime? _fec_factura,Int64 _id_cliente,string _ruc_cliente, string _nombre_cliente, string _email_cliente,
        decimal _total_factura,decimal _iva_factura, decimal _porc_iva, string _direccion_archivo, string _mensaje_archivo)
        {
            this.nc_id = _nc_id;
            this.nc_date = _nc_date;
            this.nc_concept= _nc_concept;
            this.nc_number = _nc_number;
            this.nc_authorization = _nc_authorization;
            this.id_concept= _id_concept;
            this.nc_state = _nc_state;
            this.nc_subtotal =  _nc_subtotal;
            this.nc_iva = _nc_iva;
            this.nc_total = _nc_total;
            this.nc_valor_nivel = _nc_valor_nivel;
            this.id_level = _id_level;
            this.id_factura = _id_factura;
            this.num_factura = _num_factura;
            this.fec_factura = _fec_factura;

            this.id_cliente = _id_cliente;
            this.ruc_cliente = _ruc_cliente;
            this.nombre_cliente = _nombre_cliente;
            this.email_cliente = _email_cliente;
            this.total_factura = _total_factura;
            this.iva_factura = _iva_factura;
            this.porc_iva = _porc_iva;
            this.direccion_archivo = _direccion_archivo;
            this.mensaje_archivo = _mensaje_archivo;

            this.Detalle = new List<credit_detail>();
            this.DetalleNivel = new List<credit_level_approval>();
            OnInit();
    }

        public credit_head(Int64 _nc_id)
        {
            OnInit();
            this.nc_id = _nc_id;
            this.init_list();
        }

        public credit_head()
        {
            OnInit();
            this.Detalle = new List<credit_detail>();
            this.DetalleNivel = new List<credit_level_approval>();
            this._DetalleLoaded = false;
        }

        private void init_list()
        {
            this.Detalle = new List<credit_detail>();
            this._DetalleLoaded = false;
        }


        /*conexion nota de credito*/
        private static void OnInit()
        {
            sql_pointer = (sql_pointer == null) ? SQLHandler.sql_handler.GetInstance() : sql_pointer;
            parametros = new Dictionary<string, object>();
            v_conexion = Extension.Nueva_Conexion("NOTA_CREDITO");
        }

        /*conexion nota de credito*/
        private static void OnInit_N4()
        {
            sql_pointer = (sql_pointer == null) ? SQLHandler.sql_handler.GetInstance() : sql_pointer;
            parametros = new Dictionary<string, object>();
            v_conexion = Extension.Nueva_Conexion("PORTAL_N4");
        }

        /*validaciones*/
        private int? PreValidationsTransaction(out string msg)
        {
            //validaciones de cabecera.
            if (this.nc_id <= 0)
            {
                msg = "Especifique el id de la nota de crédito";
                return 0;
            }

            if (this.id_concept <= 0)
            {
                msg = "Especifique el concepto de la nota de crédito";
                return 0;
            }

            if (!this.nc_date.HasValue)
            {

                msg = "La fecha de la transacción no es valida";
                return 0;
            }

            if (this.id_factura <= 0)
            {
                msg = "Especifique el id de la factura a la cual se aplicara  la nota de crédito";
                return 0;
            }

            if (string.IsNullOrEmpty(this.num_factura))
            {
                msg = "Debe especificar el numero de la factura a la cual se aplicara  la nota de crédito";
                return 0;

            }

            if (!this.fec_factura.HasValue)
            {

                msg = "La fecha de la factura a la cual se aplicara  la nota de crédito, no es valida";
                return 0;
            }

            if (this.id_cliente <= 0)
            {
                msg = "Especifique el id del cliente  de la factura, para la nota de crédito";
                return 0;
            }

            if (string.IsNullOrEmpty(this.ruc_cliente))
            {
                msg = "Debe especificar el ruc del cliente, para la nota de crédito";
                return 0;

            }

            if (string.IsNullOrEmpty(this.nombre_cliente))
            {
                msg = "Debe especificar el nombre del cliente, para la nota de crédito";
                return 0;

            }

            if (string.IsNullOrEmpty(this.nc_concept))
            {
                msg = "Debe especificar el concepto de la nota de crédito";
                return 0;

            }

            if (this.nc_subtotal <= 0)
            {
                msg = "Especifique el subtotal de la nota de crédito, no puede ser cero";
                return 0;
            }

            if (this.nc_total <= 0)
            {
                msg = "Especifique el total de la nota de crédito, no puede ser cero";
                return 0;
            }

            if (this.nc_total > (this.total_factura + this.iva_factura) )
            {
                msg = "El total de la nota de crédito, no puede ser mayor que la factura";
                return 0;
            }

            if (string.IsNullOrEmpty(this.Create_user))
            {
                msg = "Debe especificar el usuario que crea la nota de crédito";
                return 0;
            }

           
            //cuenta solo los activos
            var nRegistros = this.Detalle.Where(d => d.nc_subtotal != 0).Count();

            if (this.Detalle == null || nRegistros <= 0)
            {
                msg = "No se puede agregar el detalle de la transaccion, ingrese valores de la nota de credito";
                return 0;
            }

            if (this.Action != "I")/*si es diferente de nuevo registro*/
            {
                nRegistros = this.DetalleNivel.Where(d => d.nc_id != 0).Count();
                if (this.DetalleNivel == null || nRegistros <= 0)
                {
                    msg = "No se puede agregar el detalle de los usuarios que aprobaran la transaccion";
                    return 0;
                }
            }

            msg = string.Empty;
            return 1;
        }

        private Int64? Save(out string OnError)
        {

            parametros.Clear();
            parametros.Add("i_nc_id", this.nc_id);
            parametros.Add("i_nc_date", this.nc_date);
            parametros.Add("i_nc_concept", this.nc_concept);
            parametros.Add("i_nc_number", this.nc_number);
            parametros.Add("i_nc_authorization", this.nc_authorization);
            parametros.Add("i_id_concept", this.id_concept);
            parametros.Add("i_nc_state", this.nc_state);
            parametros.Add("i_nc_subtotal", this.nc_subtotal);
            parametros.Add("i_nc_iva", this.nc_iva);
            parametros.Add("i_nc_total", this.nc_total);
            parametros.Add("i_nc_valor_nivel", this.nc_valor_nivel);
            parametros.Add("i_id_level", this.id_level);
            parametros.Add("i_id_factura", this.id_factura);
            parametros.Add("i_num_factura", this.num_factura);
            parametros.Add("i_fec_factura", this.fec_factura);
            parametros.Add("i_id_cliente", this.id_cliente);
            parametros.Add("i_ruc_cliente", this.ruc_cliente);
            parametros.Add("i_nombre_cliente", this.nombre_cliente);
            parametros.Add("i_email_cliente", this.email_cliente);
            parametros.Add("i_total_factura", this.total_factura);
            parametros.Add("i_iva_factura", this.iva_factura);
            parametros.Add("i_porc_iva", this.porc_iva);

            parametros.Add("i_create_user", Create_user);
            parametros.Add("i_action", this.Action);

            var db = sql_pointer.ExecuteInsertUpdateDeleteReturn(v_conexion, 4000, "nc_i_credit_head", parametros, out OnError);
            if (!db.HasValue || db.Value < 0)
            {
                return null;
            }
            OnError = string.Empty;
            return db.Value;

        }

        private Int64? Save_Documents(out string OnError)
        {

            parametros.Clear();
            parametros.Add("i_nc_id", this.nc_id);
            parametros.Add("i_xmlDocumentos", this.xmlDocumentos);
            parametros.Add("i_create_user", Create_user);
            var db = sql_pointer.ExecuteInsertUpdateDeleteReturn(v_conexion, 4000, "nc_i_credit_document", parametros, out OnError);
            if (!db.HasValue || db.Value < 0)
            {
                return null;
            }
            OnError = string.Empty;
            return db.Value;

        }

        private Int64? Save_Email(out string OnError)
        {

            parametros.Clear();
            parametros.Add("i_nc_id", this.nc_id);
           
            var db = sql_pointer.ExecuteInsertUpdateDeleteReturn(v_conexion, 4000, "nc_c_envio_email_credit_head", parametros, out OnError);
            if (!db.HasValue || db.Value < 0)
            {
                return null;
            }
            OnError = string.Empty;
            return db.Value;

        }

        public Int64? SaveTransaction(out string OnError)
        {
            bool result_ = false;
            string resultado_others = null;
            try
            {
                //validaciones previas
                if (this.PreValidationsTransaction(out OnError) != 1)
                {
                    return 0;
                }
                using (var scope = new System.Transactions.TransactionScope())
                {
                    //grabar cabecera.
                    var id = this.Save(out OnError);
                    if (!id.HasValue)
                    {
                        return 0;
                    }
                    this.nc_id = id.Value;
                    var nContador = 1;
                    //si no falla la cabecera entonces añada los items
                    foreach (var i in this.Detalle)
                    {
                        i.nc_id = id.Value;
                        i.sequence = nContador;
                        i.Create_user = this.Create_user;

                        var IdRetorno = i.Save(out OnError);
                        if (!IdRetorno.HasValue || IdRetorno.Value <= 0)
                        {
                            return 0;
                        }

                        i.nc_id = IdRetorno.Value;
                        nContador = nContador + 1;
                    }
                    //si no falla la cabecera entonces añada los items
                    foreach (var dn in this.DetalleNivel)
                    {
                        dn.nc_id = id.Value;
                        dn.Create_user = this.Create_user;
                        dn.nc_total = this.nc_total;
                        dn.id_concept = this.id_concept; 
                        var IdRetorno = dn.Save(out OnError);
                        if (!IdRetorno.HasValue || IdRetorno.Value <= 0)
                        {
                            if (OnError == string.Empty)
                            {
                                OnError = "*** No existen niveles de aprobación para los criterios ingresados ****";
                            }
                            return 0;
                        }

                        dn.nc_id = IdRetorno.Value;
                        nContador = nContador + 1;
                    }
                    //graba documentos
                    if (this.xmlDocumentos.Length != 0) {

                        //grabar documentos.
                        var idDoc = this.Save_Documents(out OnError);
                        if (!idDoc.HasValue || idDoc.Value <= 0)
                        {
                            return 0;
                        }

                    }

                    OnError = string.Empty;
                    //grabar y enviar emial.
                    var id_mail = this.Save_Email(out OnError);
                    if (!id_mail.HasValue || id_mail.Value <= 0)
                    {
                        if (OnError == string.Empty)
                        {
                            OnError = "*** Error al enviar emial de nota de crédito ****";
                        }
                    }
                    //fin de la transaccion
                    scope.Complete();
                   
                    
                   
                    return this.nc_id;
                }
            }
            catch (Exception ex)
            {
                result_ = false;
                resultado_others = ex.Message; ;
                var r = SQLHandler.sql_handler.LogEvent<Exception>(this.Create_user, nameof(credit_head), nameof(SaveTransaction), result_, this.nc_id.ToString(), null, resultado_others, ex);
                OnError = string.Format("Ha ocurrido la excepción #{0}", r);
                return null;
            }
        }


        /*carga datos de la cabecera de la factura*/
        public static List<credit_head> Get_Cabecera_Factura(string _numero_factura)
        {
            OnInit();
            string msg;
            parametros.Clear();
            parametros.Add("i_numero_factura", _numero_factura);
            return sql_pointer.ExecuteSelectControl<credit_head>(v_conexion, 2000, "nc_get_cabecera_factura", parametros, out msg);
        }

        /*carga datos de la cabecera de la nota de credito*/
        public static List<credit_head> Get_credit_head(Int64 _nc_id)
        {
            OnInit();
            string msg;
            parametros.Clear();
            parametros.Add("i_nc_id", _nc_id);
            return sql_pointer.ExecuteSelectControl<credit_head>(v_conexion, 2000, "nc_get_credit_head", parametros, out msg);
        }

        /*valida que no exista una nota de credito aplicada a una factura*/
        public string Validate_credit_head(string _numero_factura)
        {
            string OnError;
            parametros.Clear();
            parametros.Add("i_num_factura", _numero_factura);

            var db = sql_pointer.ExecuteSelectOnly(v_conexion, 4000, "nc_validate_credit_head", parametros);
            if (db == null)
            {
                OnError = string.Format("Erro al validar nota de crédito para la factura: {0}", _numero_factura);
                return null;
            }
            else
            {
                if (db.code == 1)
                {
                    OnError = db.message;
                }
                else
                {
                    OnError = string.Empty;
                }

            }
            return OnError;

        }

        /*carga datos de la cabecera de la nota de credito*/
        public bool PopulateMyData(out string OnError)
        {
            //cargar todos los datos este documento
            if (this.nc_id <= 0)
            {
                OnError = "Debe establecer el campo nc_id de la nota de credito";
                return false;
            }
            parametros.Clear();
            parametros.Add("i_nc_id", this.nc_id);

            var t = sql_pointer.ExecuteSelectOnly<credit_head>(v_conexion, 4000, "nc_get_credit_head", parametros);
            if (t == null)
            {
                OnError = "No fue posible obtener los datos de la visita";
                return false;
            }

            this.nc_id = t.nc_id;
            this.nc_date = t.nc_date;
            this.nc_concept = t.nc_concept;
            this.nc_number = t.nc_number;
            this.nc_authorization = t.nc_authorization;
            this.id_concept = t.id_concept;
            this.nc_state = t.nc_state;
            this.nc_subtotal = t.nc_subtotal;
            this.nc_iva = t.nc_iva;
            this.nc_total = t.nc_total;
            this.nc_valor_nivel = t.nc_valor_nivel;
            this.id_level = t.id_level;
            this.id_factura = t.id_factura;
            this.num_factura = t.num_factura;
            this.fec_factura = t.fec_factura;
            this.description = t.description;
            this.id_cliente = t.id_cliente;
            this.ruc_cliente = t.ruc_cliente;
            this.nombre_cliente = t.nombre_cliente;
            this.email_cliente = t.email_cliente;
            this.total_factura = t.total_factura;
            this.iva_factura = t.iva_factura;
            this.porc_iva = t.porc_iva;
            this.Create_user = t.Create_user;
            this.Create_date = t.Create_date;
            OnError = string.Empty;
            return true;
        }

        public void LoadDetalle()
        {
            if (this.nc_id <= 0)
            {
                return;
            }

            //detalle de nota de credito
            string error;
            if (this.Detalle == null || this.Detalle.Count <= 0)
                this.Detalle = credit_detail.Get_credit_detail(this.nc_id, out error);

            this._DetalleLoaded = this.Detalle != null && this.Detalle.Count > 0 ? true : false;
        }

        /*carga todas las notas de credito pendientes de aprobar de un uusario*/
        public static List<credit_head> List_Nota_Credito_Pendientes(Int64 i_IdUsuario, string i_Usuario, DateTime i_Desde, DateTime i_Hasta, Int64 i_nc_id, int i_id_concept,   out string OnError)
        {
            OnInit();
            parametros.Clear();
            parametros.Add("i_IdUsuario", i_IdUsuario);
            parametros.Add("i_Usuario", i_Usuario);
            parametros.Add("i_Desde", i_Desde);
            parametros.Add("i_Hasta", i_Hasta);
            parametros.Add("i_nc_id", i_nc_id);
            parametros.Add("i_id_concept", i_id_concept);
            return sql_pointer.ExecuteSelectControl<credit_head>(v_conexion, 2000, "nc_c_credit_pendiente_aprobar", parametros, out OnError);
        }

        /*carga todas las notas de credito pendientes de aprobar de un usuario en todas sus etapas*/
        public static List<credit_head> List_Seguimiento_Nota_Credito(Int64 i_IdUsuario, string i_Usuario, int i_id_concept, out string OnError)
        {
            OnInit();
            parametros.Clear();
            parametros.Add("i_IdUsuario", i_IdUsuario);
            parametros.Add("i_Usuario", i_Usuario);
            parametros.Add("i_id_concept", i_id_concept);
            return sql_pointer.ExecuteSelectControl<credit_head>(v_conexion, 2000, "nc_c_list_credit_pendiente", parametros, out OnError);
        }

        /*lista las notas de credito (pendientes o aprobadas de forma resumida)*/
        public static List<credit_head> List_Nota_Credito_Resumidas(Int64 i_Estado, DateTime i_Desde, DateTime i_Hasta, int i_id_concept, out string OnError)
        {
            OnInit();
            parametros.Clear();
            parametros.Add("i_Estado", i_Estado);
            parametros.Add("i_Desde", i_Desde);
            parametros.Add("i_Hasta", i_Hasta);
            parametros.Add("i_id_concept", i_id_concept);
            return sql_pointer.ExecuteSelectControl<credit_head>(v_conexion, 2000, "nc_c_list_credit_general", parametros, out OnError);
        }

        /*aprobar notas de creditos*/
        public bool Aprobar()
        {
            OnInit();

            parametros.Clear();
            string OnError;
            parametros.Add("i_nc_id", this.nc_id);
            parametros.Add("i_id_level", this.id_level);
            parametros.Add("i_id_group", this.id_group);
            parametros.Add("i_IdUsuario", this.IdUsuario);
            parametros.Add("i_level", this.level);
            parametros.Add("i_create_user", this.Create_user);
            parametros.Add("i_xmlDocumentos", this.ruta_documento);
            using (var scope = new System.Transactions.TransactionScope())
            {
                var db = sql_pointer.ExecuteInsertUpdateDelete(v_conexion, 4000, "nc_u_credit_aprobar", parametros, out OnError);
                if (!db.HasValue || db.Value < 0)
                {
                    return false;
                }
                scope.Complete();
            }
            return true;
        }

        /*anular notas de creditos*/
        public bool Anular()
        {
            OnInit();

            parametros.Clear();
            string OnError;
            parametros.Add("i_nc_id", this.nc_id);
            parametros.Add("i_create_user", this.Create_user);
            parametros.Add("i_nc_anulacion", this.nc_anulacion);
            using (var scope = new System.Transactions.TransactionScope())
            {
                var db = sql_pointer.ExecuteInsertUpdateDelete(v_conexion, 4000, "nc_d_credit_head", parametros, out OnError);
                if (!db.HasValue || db.Value < 0)
                {
                    return false;
                }
                scope.Complete();
            }
            return true;
        }

        /*valida que no exista nada pendiente de aprobar, para transmitir al N4 Billing*/
        public string Validate_credit_head_Pendiente(Int64 _nc_id)
        {
            string OnError;
            parametros.Clear();
            parametros.Add("i_nc_id", _nc_id);

            var db = sql_pointer.ExecuteSelectOnly(v_conexion, 4000, "nc_existe_credit_head_pendiente", parametros);
            if (db == null)
            {
                OnError = string.Format("Error al validar nota de crédito pendiente de aprobar #: {0}", _nc_id.ToString().Trim());
                return null;
            }
            else
            {               
               OnError = db.message;  
            }

            return OnError;

        }

        /*xml para transmitir al N4 Billing*/
        public string Genera_XML_credit(Int64 _nc_id, string _i_accion, string i_draft_nbr)
        {
            string OnError;
            parametros.Clear();
            parametros.Add("i_nc_id", _nc_id);
            parametros.Add("i_accion", _i_accion);
            parametros.Add("i_draft_nbr", i_draft_nbr);

            var db = sql_pointer.ExecuteSelectOnly(v_conexion, 4000, "nc_genera_xml_credit", parametros);
            if (db == null)
            {
                OnError = string.Format("ERROR");
                return null;
            }
            else
            { 
              OnError = db.message;
            }

            return OnError;

        }

        /*aprobar notas de creditos*/
        public bool Actualizar_XML()
        {
            OnInit();

            parametros.Clear();
            string OnError;
            parametros.Add("i_nc_id", this.nc_id);
            parametros.Add("i_xml", this.nc_xml_draft);
            parametros.Add("i_tipo", this.tipo);
            parametros.Add("i_nc_number", this.nc_number);
            using (var scope = new System.Transactions.TransactionScope())
            {
                var db = sql_pointer.ExecuteInsertUpdateDelete(v_conexion, 4000, "nc_u_credit_aprobar_xml", parametros, out OnError);
                if (!db.HasValue || db.Value < 0)
                {
                    return false;
                }
                scope.Complete();
            }
            return true;
        }


        /*reverso de aprobacion de notas de creditos*/
        public bool Reverso()
        {
            OnInit();

            parametros.Clear();
            string OnError;
            parametros.Add("i_nc_id", this.nc_id);
            parametros.Add("i_id_level", this.id_level);
            parametros.Add("i_id_group", this.id_group);
            parametros.Add("i_IdUsuario", this.IdUsuario);
            parametros.Add("i_level", this.level);
            parametros.Add("i_create_user", this.Create_user);
            using (var scope = new System.Transactions.TransactionScope())
            {
                var db = sql_pointer.ExecuteInsertUpdateDelete(v_conexion, 4000, "nc_u_credit_reverso", parametros, out OnError);
                if (!db.HasValue || db.Value < 0)
                {
                    return false;
                }
                scope.Complete();
            }
            return true;
        }


        /*listado de archivos a exportar*/
        public static List<credit_head> List_Archivos(Int64 _nc_id, out string OnError)
        {
            OnInit();
            parametros.Clear();
            parametros.Add("i_nc_id", _nc_id);  
            return sql_pointer.ExecuteSelectControl<credit_head>(v_conexion, 2000, "nc_c_list_file_credit", parametros, out OnError);
        }

    }
}
