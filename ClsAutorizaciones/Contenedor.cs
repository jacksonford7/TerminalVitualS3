using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ControlOPC.Entidades;


namespace ClsAutorizaciones
{
   public class Contenedor : Base
    {
        #region "Variables"

        private Int64 _CNTR_CONSECUTIVO;
        private string _CNTR_CONTAINER = string.Empty;
        private string _CNTR_VEPR_REFERENCE = string.Empty;
        private string _CNTR_GROUP = string.Empty;
        private string _CNTR_LINE = string.Empty;
        private bool _CNTR_VALIDACION_REF = false;
        private bool _CNTR_VALIDACION_LINE = false;
        private string _CNTR_NUMAUTSENAE = string.Empty;
        private bool _CNTR_VAL_N4 = false;
        private string _CNTR_ERROR_N4 = string.Empty;
        private string _REF_FINAL = string.Empty;
        private string _BOOKING = string.Empty;

        private Int64 _ID=0;
        private DateTime? _FECHA;
        private string _AUTORIZACION = string.Empty;
        private string _REFERENCIA = string.Empty;
        private string _ARCHIVO = string.Empty;
        private bool _ESTADO = false;
        private string _PROCESO = string.Empty;
        private string _USUARIO_CRE = string.Empty;
        private DateTime? _FECHA_CREA;
        private string _USUARIO_MOD = string.Empty;
        private DateTime? _FECHA_MOD;
        private string _LINEA_NAVIERA = string.Empty;
        private string _CONTENEDOR = string.Empty;
        private string _MAIL = string.Empty;

        private Int64 _SECUENCIA = 0;
        private string _MENSAJE = string.Empty;
        private string _TIPO = string.Empty;
        private int _CANTIDAD_AUTORIZADA = 0;
        #endregion

        #region "Propiedades"
        public Int64 CNTR_CONSECUTIVO { get => _CNTR_CONSECUTIVO; set => _CNTR_CONSECUTIVO = value; }
        public string CNTR_CONTAINER { get => _CNTR_CONTAINER; set => _CNTR_CONTAINER = value; }
        public string CNTR_VEPR_REFERENCE { get => _CNTR_VEPR_REFERENCE; set => _CNTR_VEPR_REFERENCE = value; }
        public string CNTR_GROUP { get => _CNTR_GROUP; set => _CNTR_GROUP = value; }
        public string CNTR_LINE { get => _CNTR_LINE; set => _CNTR_LINE = value; }
        public bool CNTR_VALIDACION_REF { get => _CNTR_VALIDACION_REF; set => _CNTR_VALIDACION_REF = value; }
        public bool CNTR_VALIDACION_LINE { get => _CNTR_VALIDACION_LINE; set => _CNTR_VALIDACION_LINE = value; }
        public string CNTR_NUMAUTSENAE { get => _CNTR_NUMAUTSENAE; set => _CNTR_NUMAUTSENAE = value; }
        public bool CNTR_VAL_N4 { get => _CNTR_VAL_N4; set => _CNTR_VAL_N4 = value; }
        public string CNTR_ERROR_N4 { get => _CNTR_ERROR_N4; set => _CNTR_ERROR_N4 = value; }
        public string REF_FINAL { get => _REF_FINAL; set => _REF_FINAL = value; }
        public string BOOKING { get => _BOOKING; set => _BOOKING = value; }

        public Int64 ID { get => _ID; set => _ID = value; }
        public DateTime? FECHA { get => _FECHA; set => _FECHA = value; }
        public string AUTORIZACION { get => _AUTORIZACION; set => _AUTORIZACION = value; }
        public string REFERENCIA { get => _REFERENCIA; set => _REFERENCIA = value; }
        public string ARCHIVO { get => _ARCHIVO; set => _ARCHIVO = value; }
        public bool ESTADO { get => _ESTADO; set => _ESTADO = value; }
        public string PROCESO { get => _PROCESO; set => _PROCESO = value; }
        public string USUARIO_CRE { get => _USUARIO_CRE; set => _USUARIO_CRE = value; }
        public DateTime? FECHA_CREA { get => _FECHA_CREA; set => _FECHA_CREA = value; }
        public string USUARIO_MOD { get => _USUARIO_MOD; set => _USUARIO_MOD = value; }
        public DateTime? FECHA_MOD { get => _FECHA_MOD; set => _FECHA_MOD = value; }
        public string LINEA_NAVIERA { get => _LINEA_NAVIERA; set => _LINEA_NAVIERA = value; }
        public string CONTENEDOR { get => _CONTENEDOR; set => _CONTENEDOR = value; }
        public string MAIL { get => _MAIL; set => _MAIL = value; }

        public Int64 SECUENCIA { get => _SECUENCIA; set => _SECUENCIA = value; }
        public string MENSAJE { get => _MENSAJE; set => _MENSAJE = value; }
        public string TIPO { get => _TIPO; set => _TIPO = value; }
        public int CANTIDAD_AUTORIZADA { get => _CANTIDAD_AUTORIZADA; set => _CANTIDAD_AUTORIZADA = value; }

        #endregion

        private static String v_mensaje = string.Empty;

        public List<Contenedor_Detalle> Detalle { get; set; }

      public Contenedor()
        {
            init();
            this.Detalle = new List<Contenedor_Detalle>();
            OnInit();
        }

        #region "Metodos"
        private static void OnInit()
        {
            sql_pointer = (sql_pointer == null) ? SQLHandler.sql_handler.GetInstance() : sql_pointer;
            parametros = new Dictionary<string, object>();

            v_conexion = Extension.Nueva_Conexion("PORTAL_N4");
        }

        public static List<Contenedor> CONSULTA_CONTENEDORES(string xml, out string OnError)
        {
            OnInit();

            parametros.Clear();
            parametros.Add("unit_all", xml);
            return sql_pointer.ExecuteSelectControl<Contenedor>(v_conexion, 6000, "RVA_FNA_FUN_EDO_CONTAINER", parametros, out OnError);
           /* return sql_pointer.ExecuteSelectControl<Contenedor>(v_conexion, 6000, "FNA_FUN_EDO_CONTAINER_N5", parametros, out OnError);*/

        }

        public static List<Contenedor> CONSULTA_ERRORES_CONTENEDORES( out string OnError)
        {
            OnInit();

            return sql_pointer.ExecuteSelectControl<Contenedor>(v_conexion, 6000, "RVA_AUTORIZA_LOG_ERRORES_CONTENEDORES", null, out OnError);

        }


        public static List<Contenedor> VALIDA_CONTENEDORES(string xml, out string OnError)
        {
            OnInit();

            parametros.Clear();
            parametros.Add("XmlContenedor", xml);
            return sql_pointer.ExecuteSelectControl<Contenedor>(v_conexion, 6000, "RVA_VALIDA_ORDEN_SERVICIO", parametros, out OnError);

        }


        private int? PreValidations(out string msg)
        {

            if (string.IsNullOrEmpty(this.AUTORIZACION))
            {
                msg = "Debe especificar el # de autorización";
                return 0;
            }

            if (string.IsNullOrEmpty(this.REFERENCIA))
            {
                msg = "Debe especificar la referencia";
                return 0;
            }

            if (string.IsNullOrEmpty(this.ARCHIVO))
            {
                msg = "Debe especificar el archivo a cargar";
                return 0;
            }

            if (string.IsNullOrEmpty(this.LINEA_NAVIERA))
            {
                msg = "Debe especificar la línea naviera";
                return 0;
            }
            if (string.IsNullOrEmpty(this.USUARIO_CRE))
            {
                msg = "Especifique el usuario creador del registro";
                return 0;
            }

            msg = string.Empty;
            return 1;

        }

        public Int64? Save(out string OnError)
        {
            if (this.PreValidations(out OnError) != 1)
            {
                return -1;
            }

            parametros.Clear();
            parametros.Add("AUTORIZACION", this.AUTORIZACION);
            parametros.Add("REFERENCIA", this.REFERENCIA);
            parametros.Add("ARCHIVO", this.ARCHIVO);
            parametros.Add("ESTADO", this.ESTADO);
            parametros.Add("USUARIO_CRE", this.USUARIO_CRE);
            parametros.Add("LINEA_NAVIERA", this.LINEA_NAVIERA);
            parametros.Add("MAIL", this.MAIL);
            parametros.Add("CANTIDAD_AUTORIZADA", this.CANTIDAD_AUTORIZADA);
            var db = sql_pointer.ExecuteInsertUpdateDeleteReturn(v_conexion, 6000, "RVA_GRABA_CONTENEDOR_CAB", parametros, out OnError);
            if (!db.HasValue || db.Value < 0)
            {

                return null;
            }
            OnError = string.Empty;
               
            return db.Value;
         

        }


        public Int64? SaveTransaction(out string OnError)
        {

            string resultado_otros = null;
            Int64 ID = 0;
            try
            {
                
                using (var scope = new System.Transactions.TransactionScope())
                {
                    //grabar cabecera de la transaccion.
                    var id = this.Save(out OnError);
                    if (!id.HasValue)
                    {
                        return 0;
                    }

                    this.ID = id.Value;
                    ID = id.Value;
                    var nContador = 1;

                    //si no falla la cabecera entonces añado detalle de contenedores
                    foreach (var i in this.Detalle)
                    {
                        i.ID = ID;
                       
                        var IdRetorno = i.Save(out OnError);
                        if (!IdRetorno.HasValue || IdRetorno.Value <= 0)
                        {
                            OnError = "*** Error: al grabar detalle de transacción ****";
                            return 0;
                        }

                        nContador = nContador + 1;

                    }

                    //fin de la transaccion
                    scope.Complete();

                    return ID;
                }
            }
            catch (Exception ex)
            {
                resultado_otros = ex.Message; ;
                OnError = string.Format("Ha ocurrido la excepción #{0}", resultado_otros);
                return null;
            }
        }


        public bool Activar(out string OnError)
        {
            parametros.Clear();
            parametros.Add("ID", this.ID);
            parametros.Add("SECUENCIA", this.SECUENCIA);
            parametros.Add("TIPO", this.TIPO);
            using (var scope = new System.Transactions.TransactionScope())
            {
                var db = sql_pointer.ExecuteInsertUpdateDelete(v_conexion, 6000, "RVA_ACTIVA_CONTENEDOR_EDO", parametros, out OnError);
                if (!db.HasValue || db.Value < 0)
                {
                    return false;
                }
                scope.Complete();
            }
            return true;
        }

        public bool Activar_Unidades_PorLote(string Xml, out string OnError)
        {
            parametros.Clear();
            parametros.Add("XML", Xml);
           
            using (var scope = new System.Transactions.TransactionScope())
            {
                var db = sql_pointer.ExecuteInsertUpdateDelete(v_conexion, 6000, "RVA_ACTIVA_LOTE_CONTENEDOR_EDO", parametros, out OnError);
                if (!db.HasValue || db.Value < 0)
                {
                    return false;
                }
                scope.Complete();
            }
            return true;
        }

        public bool Eliminar_Unidades_PorLote(string Xml, out string OnError)
        {
            parametros.Clear();
            parametros.Add("XML", Xml);

            using (var scope = new System.Transactions.TransactionScope())
            {
                var db = sql_pointer.ExecuteInsertUpdateDelete(v_conexion, 6000, "RVA_ELIMINAR_LOTE_CONTENEDOR_EDO", parametros, out OnError);
                if (!db.HasValue || db.Value < 0)
                {
                    return false;
                }
                scope.Complete();
            }
            return true;
        }

        #endregion

    }
}
