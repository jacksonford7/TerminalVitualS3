using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlConexion;
using BillionEntidades;

namespace BillionAccesoDatos
{
    public class DA_Bill_InvoiceContainerExpo : Cls_Bil_Base
    {

        #region "Variables"

    
        #endregion

        #region "Propiedades"

      
        private static String v_mensaje = string.Empty;


        #endregion

        public Cls_Bill_CabeceraExpo Cabecera { get; set; }

        public DA_Bill_InvoiceContainerExpo()
        {
            init();
            this.Cabecera = new Cls_Bill_CabeceraExpo();
        }

        public DA_Bill_InvoiceContainerExpo(Cls_Bill_CabeceraExpo _cabecera)

        {
            this.Cabecera = _cabecera;
        }


        private int? PreValidationsTransaction(out string msg)
        {
            if (string.IsNullOrEmpty(this.Cabecera.CNTR_VEPR_REFERENCE))
            {
                msg = "No existe referencia de nave";
                return 0;
            }
            msg = string.Empty;
            return 1;
        }

        private Int64? Save(out string OnError)
        {

#if DEBUG
            if (this.Cabecera.CNTR_VEPR_ACTUAL_DEPARTED == null)
            {
                this.Cabecera.CNTR_VEPR_ACTUAL_DEPARTED =DateTime.Now;
                
            }
#endif

            parametros.Clear();
            parametros.Add("i_CNTR_VEPR_REFERENCE", this.Cabecera.CNTR_VEPR_REFERENCE);
            parametros.Add("i_CNTR_BKNG_BOOKING", this.Cabecera.CNTR_BKNG_BOOKING);
            parametros.Add("i_CNTR_CLIENT_ID", this.Cabecera.CNTR_CLIENT_ID);
            parametros.Add("i_CNTR_CLIENT", this.Cabecera.CNTR_CLIENTE.Cliente);
            parametros.Add("i_CNTR_INVOICE_TYPE", this.Cabecera.CNTR_INVOICE_TYPE);
            parametros.Add("i_CNTR_INVOICE_TYPE_NAME", this.Cabecera.CNTR_INVOICE_TYPE_NAME);
            parametros.Add("i_CNTR_CONTAINERS", this.Cabecera.CNTR_CONTAINERSXML);
            parametros.Add("i_CNTR_FECHA", this.Cabecera.CNTR_FECHA);
            parametros.Add("i_CNTR_ESTADO", this.Cabecera.CNTR_ESTADO);
            parametros.Add("i_CNTR_VSSL_NAME", this.Cabecera.CNTR_VEPR_VSSL_NAME);
            parametros.Add("i_CNTR_VOYAGE", this.Cabecera.CNTR_VEPR_VOYAGE);
            parametros.Add("i_CNTR_ARRIVAL", this.Cabecera.CNTR_VEPR_ACTUAL_ARRIVAL);
            parametros.Add("i_CNTR_DEPARTE", this.Cabecera.CNTR_VEPR_ACTUAL_DEPARTED);
            parametros.Add("i_CNTR_USUARIO_CREA", this.Cabecera.CNTR_USUARIO_CREA);
            parametros.Add("i_CNTR_FECHA_CREA", DateTime.Now);
            parametros.Add("i_CNTR_CREDITO", this.Cabecera.CNTR_CREDITO);
            parametros.Add("i_CNTR_CONTENEDOR20", this.Cabecera.CNTR_CONTENEDOR20);
            parametros.Add("i_CNTR_CONTENEDOR40", this.Cabecera.CNTR_CONTENEDOR40);
            parametros.Add("i_CNTR_TIPO_CLIENTE", this.Cabecera.CNTR_CLIENTE.Tipo);
            parametros.Add("i_CNTR_CLIENTE_EMAIL", this.Cabecera.CNTR_CLIENTE.DatoCliente?.CLNT_FAX_INVC);
            parametros.Add("i_CNTR_CLIENTE_CIUDAD", this.Cabecera.CNTR_CLIENTE.DatoCliente?.CLNT_CITY);
            parametros.Add("i_CNTR_CLIENTE_DIRECCION", this.Cabecera.CNTR_CLIENTE.DatoCliente?.CLNT_ADRESS);
            parametros.Add("i_CNTR_CLIENTE_DIAS_CRED", this.Cabecera.CNTR_CLIENTE.DatoCliente?.DIAS_CREDITO);


            var db = sql_puntero.ExecuteInsertUpdateDeleteReturn(sql_puntero.Conexion_Local, 4000, "sp_bil_inserta_container_expo_cab", parametros, out OnError);
            if (!db.HasValue || db.Value < 0)
            {
                return null;
            }
            OnError = string.Empty;
            this.Cabecera.CNTR_ID = db.Value; 
            return db.Value;

        }

        public Int64? SaveDetail(Cls_Bill_Container_Expo objDetalle ,out string OnError)
        {
#if DEBUG
            if (objDetalle.CNTR_VEPR_ACTUAL_DEPARTED == null)
            {
                objDetalle.CNTR_VEPR_ACTUAL_DEPARTED = DateTime.Now;
            }
#endif
            parametros.Clear();
            parametros.Add("i_CNTR_CAB_ID", objDetalle.CNTR_CAB_ID);
            parametros.Add("i_CNTR_CONSECUTIVO", objDetalle.CNTR_CONSECUTIVO);
            parametros.Add("i_CNTR_CONTAINER", objDetalle.CNTR_CONTAINER);
            parametros.Add("i_CNTR_TYPE", objDetalle.CNTR_TYPE);
            parametros.Add("i_CNTR_TYSZ_SIZE", objDetalle.CNTR_TYSZ_SIZE);
            parametros.Add("i_CNTR_TYSZ_ISO", objDetalle.CNTR_TYSZ_ISO);
            parametros.Add("i_CNTR_TYSZ_TYPE", objDetalle.CNTR_TYSZ_TYPE);
            parametros.Add("i_CNTR_FULL_EMPTY_CODE", objDetalle.CNTR_FULL_EMPTY_CODE);
            parametros.Add("i_CNTR_YARD_STATUS", objDetalle.CNTR_YARD_STATUS);
            parametros.Add("i_CNTR_TEMPERATURE", objDetalle.CNTR_TEMPERATURE);
            parametros.Add("i_CNTR_TYPE_DOCUMENT", objDetalle.CNTR_TYPE_DOCUMENT);
            parametros.Add("i_CNTR_DOCUMENT", objDetalle.CNTR_DOCUMENT);
            parametros.Add("i_CNTR_VEPR_REFERENCE", objDetalle.CNTR_VEPR_REFERENCE);
            parametros.Add("i_CNTR_CLNT_CUSTOMER_LINE", objDetalle.CNTR_CLNT_CUSTOMER_LINE);
            parametros.Add("i_CNTR_LCL_FCL", objDetalle.CNTR_LCL_FCL);
            parametros.Add("i_CNTR_CATY_CARGO_TYPE", objDetalle.CNTR_CATY_CARGO_TYPE);
            parametros.Add("i_CNTR_FREIGHT_KIND", objDetalle.CNTR_FREIGHT_KIND);
            parametros.Add("i_CNTR_DD", objDetalle.CNTR_DD );
            parametros.Add("i_CNTR_BKNG_BOOKING", objDetalle.CNTR_BKNG_BOOKING);
            parametros.Add("i_FECHA_CAS", objDetalle.FECHA_CAS);
            parametros.Add("i_CNTR_AISV", objDetalle.CNTR_AISV);
            parametros.Add("i_CNTR_HOLD", objDetalle.CNTR_HOLD);
            parametros.Add("i_CNTR_REEFER_CONT", objDetalle.CNTR_REEFER_CONT);
            parametros.Add("i_CNTR_VEPR_VSSL_NAME", objDetalle.CNTR_VEPR_VSSL_NAME);
            parametros.Add("i_CNTR_VEPR_VOYAGE", objDetalle.CNTR_VEPR_VOYAGE);
            parametros.Add("i_CNTR_VEPR_ACTUAL_ARRIVAL", objDetalle.CNTR_VEPR_ACTUAL_ARRIVAL);
            parametros.Add("i_CNTR_VEPR_ACTUAL_DEPARTE", objDetalle.CNTR_VEPR_ACTUAL_DEPARTED);
            parametros.Add("i_CNTR_PROFORMA", objDetalle.CNTR_PROFORMA);
            var db = sql_puntero.ExecuteInsertUpdateDeleteReturn(sql_puntero.Conexion_Local, 4000, "sp_bil_inserta_container_expo_det", parametros, out OnError);
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
            try
            {
                //validaciones previas
                if (this.PreValidationsTransaction(out OnError) != 1)
                {
                    return 0;
                }

                using (var scope = new System.Transactions.TransactionScope())
                {
                    //grabar cabecera de la transaccion.
                    var id = this.Save(out OnError);         
                    if (!id.HasValue)
                    {
                        return 0;
                    }

                    var nContador = 1;

                    //si no falla la cabecera entonces añado detalle de contenedores
                    foreach (var i in this.Cabecera.Detalle)
                    {
                        i.CNTR_CAB_ID  = id.Value;
                        i.CNTR_CONSECUTIVO = i.CNTR_CONSECUTIVO;
                        i.CNTR_CONTAINER = i.CNTR_CONTAINER;
                        i.CNTR_TYPE = i.CNTR_TYPE;
                        i.CNTR_TYSZ_SIZE = i.CNTR_TYSZ_SIZE;
                        i.CNTR_TYSZ_ISO = i.CNTR_TYSZ_ISO;
                        i.CNTR_TYSZ_TYPE = i.CNTR_TYSZ_TYPE;
                        i.CNTR_FULL_EMPTY_CODE = i.CNTR_FULL_EMPTY_CODE;
                        i.CNTR_YARD_STATUS = i.CNTR_YARD_STATUS;
                        i.CNTR_TEMPERATURE = i.CNTR_TEMPERATURE;
                        i.CNTR_TYPE_DOCUMENT = i.CNTR_TYPE_DOCUMENT;
                        i.CNTR_DOCUMENT = i.CNTR_DOCUMENT;
                        i.CNTR_VEPR_REFERENCE = i.CNTR_VEPR_REFERENCE;
                        i.CNTR_CLNT_CUSTOMER_LINE = i.CNTR_CLNT_CUSTOMER_LINE;
                        i.CNTR_LCL_FCL = i.CNTR_LCL_FCL;
                        i.CNTR_CATY_CARGO_TYPE = i.CNTR_CATY_CARGO_TYPE;
                        i.CNTR_FREIGHT_KIND = i.CNTR_FREIGHT_KIND;
                        i.CNTR_DD = i.CNTR_DD;
                        i.CNTR_BKNG_BOOKING = i.CNTR_BKNG_BOOKING;
                        i.FECHA_CAS = i.FECHA_CAS;
                        i.CNTR_AISV = i.CNTR_AISV;
                        i.CNTR_HOLD = i.CNTR_HOLD;
                        i.CNTR_REEFER_CONT = i.CNTR_REEFER_CONT;
                        i.CNTR_VEPR_VSSL_NAME = i.CNTR_VEPR_VSSL_NAME;
                        i.CNTR_VEPR_VOYAGE = i.CNTR_VEPR_VOYAGE;
                        i.CNTR_VEPR_ACTUAL_ARRIVAL = i.CNTR_VEPR_ACTUAL_ARRIVAL;
                        i.CNTR_VEPR_ACTUAL_DEPARTED = i.CNTR_VEPR_ACTUAL_DEPARTED;
                        i.IV_USUARIO_CREA = this.IV_USUARIO_CREA;
                        i.IV_FECHA_CREA = this.IV_FECHA_CREA;
                        i.CNTR_PROFORMA = i.CNTR_PROFORMA;

                        var IdRetorno = SaveDetail(i,out OnError);
                        if (!IdRetorno.HasValue || IdRetorno.Value <= 0)
                        {
                            OnError = "*** Error: al grabar detalle de factura ****" +" "+ OnError;
                            return 0;
                        }

                        nContador = nContador + 1;
                    }

                    //fin de la transaccion
                    scope.Complete();
                    return id.Value;
                }
            }
            catch (Exception ex)
            {
                resultado_otros = ex.Message; ;
                OnError = string.Format("Ha ocurrido la excepción #{0}", resultado_otros);
                return null;
            }
        }

        public Int64? UpdateStatus(long _codigo, string _estado, out string OnError)
        {

            parametros.Clear();
            parametros.Add("i_codigo", _codigo);
            parametros.Add("i_estado", _estado);
           
            var db = sql_puntero.ExecuteInsertUpdateDeleteReturn(sql_puntero.Conexion_Local, 4000, "sp_bil_actualiza_container_expo_cab", parametros, out OnError);
            if (!db.HasValue || db.Value < 0)
            {
                return null;
            }
            OnError = string.Empty;
            return db.Value;

        }

        public List<Cls_Bill_CabeceraExpo> ConsultaCabecerasPendientes(int _registros, string _estado, out string OnError)
        {
            parametros.Clear();
            parametros.Add("registros", _registros);
            parametros.Add("estado", _estado);

            var db = sql_puntero.ExecuteSelectControl<Cls_Bill_CabeceraExpo>(sql_puntero.Conexion_Local, 4000, "[Bill].[expo_cabecera_pendiente]", parametros, out OnError);
            return db;
        }
        
        public List<Cls_Bill_CabeceraExpo> ConsultaEntidad(string _CNTR_VEPR_REFERENCE, out string OnError)
        {
            parametros.Clear();
            parametros.Add("i_CNTR_VEPR_REFERENCE", _CNTR_VEPR_REFERENCE);
           
            var db = sql_puntero.ExecuteSelectControl<Cls_Bill_CabeceraExpo>(sql_puntero.Conexion_Local, 4000, "sp_bil_consulta_container_expo_cab", parametros,out OnError);
            return db;
        }

        public List<Cls_Bill_CabeceraExpo> ConsultaEntidad_Full(string _CNTR_VEPR_REFERENCE, out string OnError)
        {
            parametros.Clear();
            parametros.Add("i_CNTR_VEPR_REFERENCE", _CNTR_VEPR_REFERENCE);

            var db = sql_puntero.ExecuteSelectControl<Cls_Bill_CabeceraExpo>(sql_puntero.Conexion_Local, 6000, "sp_bil_consulta_container_expo_cab_full", parametros, out OnError);
            return db;
        }

        public List<Cls_Bill_CabeceraExpo> ConsultaEntidad_Pan(string _CNTR_VEPR_REFERENCE, out string OnError)
        {
            parametros.Clear();
            parametros.Add("i_CNTR_VEPR_REFERENCE", _CNTR_VEPR_REFERENCE);

            var db = sql_puntero.ExecuteSelectControl<Cls_Bill_CabeceraExpo>(sql_puntero.Conexion_Local, 6000, "sp_bil_consulta_container_expo_cab_pan", parametros, out OnError);
            return db;
        }

        public List<Cls_Bill_CabeceraExpo> ConsultaEntidad_Reefer(string _CNTR_VEPR_REFERENCE, out string OnError)
        {
            parametros.Clear();
            parametros.Add("i_CNTR_VEPR_REFERENCE", _CNTR_VEPR_REFERENCE);

            var db = sql_puntero.ExecuteSelectControl<Cls_Bill_CabeceraExpo>(sql_puntero.Conexion_Local, 6000, "sp_bil_consulta_container_expo_cab_reefer", parametros, out OnError);
            return db;
        }


        public List<Cls_Bill_Container_Expo> ConsultaDetalle(long? _CNTR_CAB_ID, out string OnError)
        {
            parametros.Clear();
            parametros.Add("i_CNTR_CAB_ID", _CNTR_CAB_ID);

            var db = sql_puntero.ExecuteSelectControl<Cls_Bill_Container_Expo>(sql_puntero.Conexion_Local, 4000, "sp_bil_consulta_container_expo_det", parametros, out OnError);
            return db;
        }
   
        public List<Cls_Bill_Container_Expo_Det_Validacion> ConsultaSubdetalle(long? _CNTR_CAB_ID, long? _CNTR_CONSECUTIVO, string _CNTR_VEPR_REFERENCE, out string OnError)
        {
            parametros.Clear();
            parametros.Add("i_CNTR_CAB_ID", _CNTR_CAB_ID);
            parametros.Add("i_CNTR_CONSECUTIVO", _CNTR_CONSECUTIVO);
            parametros.Add("i_REFERENCIA", _CNTR_VEPR_REFERENCE);

            var db = sql_puntero.ExecuteSelectControl<Cls_Bill_Container_Expo_Det_Validacion>(sql_puntero.Conexion_Local, 4000, "sp_bil_container_expo_det_validacion", parametros, out OnError);
            return db;
        }

        public List<Cls_FacturasExpo> ConsultaFacturas(string _REFERENCIA, string _BOOKING, out string OnError)
        {
            parametros.Clear();
            parametros.Add("i_REFERENCIA", _REFERENCIA);
            parametros.Add("i_BOOKING", _BOOKING);

            var db = sql_puntero.ExecuteSelectControl<Cls_FacturasExpo>(sql_puntero.Conexion_Local, 4000, "sp_Bil_ConsultarFacturasExpo", parametros, out OnError);
            return db;
        }

    }
}
