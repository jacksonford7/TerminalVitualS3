using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Respuesta;
using AccesoDatos;
using Configuraciones;
using System.Xml.Serialization;
using System.Reflection;

namespace N4.Exportacion
{
    public class expo_invoice:ModuloBase
    {
        public Int64? id { get; set; }
        public Int64? cntr_id { get; set; }
        public string boking_id { get; set; }
        public string reference_id { get; set; }
        public string changed { get; set; }
        public string created { get; set; }
        public string draftNumber { get; set; }
        public string dueDate { get; set; }
        public string effectiveDate { get; set; }
        public string isMerged { get; set; }
        public string facilityId { get; set; }
        public string facilityName { get; set; }
        public string complexName { get; set; }
        public string complexId { get; set; }
        public string finalizedDate { get; set; }
        public string finalNumber { get; set; }
        public string revenueMonth { get; set; }
        public string status { get; set; }
        public string totalCharges { get; set; }
        public string totalCredits { get; set; }
        public string totalCreditTaxes { get; set; }
        public string totalDiscounts { get; set; }
        public DateTime? created_date { get; set; }
        public string created_user { get; set; }
        public bool? register_Active { get; set; }
        public string totalTotal { get; set; }
        public string totalTaxes { get; set; }

        public List<expo_invoice_detail> details { get; set; }

        public expo_invoice()
        {
            this.details = new List<expo_invoice_detail>();
        }


        /*nuevo_bil_expo_cab_invoice*/
        public override void OnInstanceCreate()
        {
            this.alterClase = "N4";
            base.OnInstanceCreate();
            this.Accesorio.ConfiguracionBase = "DATACON";
           
        }

        public ResultadoOperacion<Int64> NuevoRegistro()
        {
            this.actualMetodo = MethodBase.GetCurrentMethod().Name;
            string pv = string.Empty;
            if (!this.Accesorio.Inicializar(out pv))
            {
                return Respuesta.ResultadoOperacion<Int64>.CrearFalla(pv);
            }
            var tt = SetMessage("NO_NULO", actualMetodo, created_user);
            //el usuario creador
            if (string.IsNullOrEmpty(created_user))
            {
                return Respuesta.ResultadoOperacion<Int64>.CrearFalla(string.Format(tt.Item1, nameof(created_user)));
            }
            //el booking
            if (string.IsNullOrEmpty(boking_id))
            {
                return Respuesta.ResultadoOperacion<Int64>.CrearFalla(string.Format(tt.Item1, nameof(boking_id)));
            }
            if (string.IsNullOrEmpty(reference_id))
            {
                return Respuesta.ResultadoOperacion<Int64>.CrearFalla(string.Format(tt.Item1, nameof(reference_id)));
            }
            //el id cabcera
            if (!this.cntr_id.HasValue)
            {
                return Respuesta.ResultadoOperacion<Int64>.CrearFalla(string.Format(tt.Item1, nameof(cntr_id)));
            }
            this.Parametros.Clear();
            this.Parametros.Add(nameof(cntr_id), cntr_id);
            this.Parametros.Add(nameof(boking_id), boking_id);
            this.Parametros.Add(nameof(reference_id), reference_id);
            this.Parametros.Add(nameof(changed), changed);
            this.Parametros.Add(nameof(created), created);
            this.Parametros.Add(nameof(draftNumber), draftNumber);
            this.Parametros.Add(nameof(dueDate), dueDate);
            this.Parametros.Add(nameof(effectiveDate), effectiveDate);
            this.Parametros.Add(nameof(isMerged), isMerged);
            this.Parametros.Add(nameof(facilityId), facilityId);
            this.Parametros.Add(nameof(facilityName), facilityName);
            this.Parametros.Add(nameof(complexName), complexName);
            this.Parametros.Add(nameof(complexId), complexId);
            this.Parametros.Add(nameof(finalizedDate), finalizedDate);
            this.Parametros.Add(nameof(finalNumber), finalNumber);
            this.Parametros.Add(nameof(revenueMonth), revenueMonth);
            this.Parametros.Add(nameof(status), status);
            this.Parametros.Add(nameof(totalCharges), totalCharges);
            this.Parametros.Add(nameof(totalCredits), totalCredits);
            this.Parametros.Add(nameof(totalCreditTaxes), totalCreditTaxes);
            this.Parametros.Add(nameof(totalDiscounts), totalDiscounts);
            this.Parametros.Add(nameof(created_user), created_user);
            this.Parametros.Add(nameof(totalTotal), totalTotal);
            this.Parametros.Add(nameof(totalTaxes), totalTaxes);

            //YA_ASIGNADO   
            //if (this.details == null || this.details.Count <= 0)
            //{
            //    return Respuesta.ResultadoOperacion<Int64>.CrearFalla("No se puede insertar cabecera sin detalles");
            //}
#if DEBUG
            this.LogEvent(created_user, this.actualMetodo, string.Format("Inicia la transaccion {0}", DateTime.Now.ToString("dd/MM/yyyy HH:mm")));
#endif
            var bcon = this.Accesorio.ObtenerConfiguracion("Billion")?.valor;
            using (var scope = new System.Transactions.TransactionScope())
            {
                try
                {
                    var result = BDOpe.ComandoInsertUpdateDeleteID(bcon, "[Bill].[nuevo_bil_expo_cab_invoice]", this.Parametros);
                    /*bill.upsert_pago_asignado*/
                    if (!result.Exitoso)
                    {
                        return Respuesta.ResultadoOperacion<Int64>.CrearFalla(result.MensajeProblema);
                    }
                    this.id = result.Resultado.Value;
                    foreach (var it in this.details)
                    {
                        this.Parametros.Clear();
                        this.Parametros.Add("bil_expo_cab_invoice_id", this.id);
                        this.Parametros.Add("totalCharged", it.totalCharged);
                        this.Parametros.Add("billingDate", it.billingDate);
                        this.Parametros.Add("description", it.description);
                        this.Parametros.Add("chargeEntityId", it.chargeEntityId);
                        this.Parametros.Add("chargeEventTypeId", it.chargeEventTypeId);
                        this.Parametros.Add("customerTariffId", it.customerTariffId);
                        this.Parametros.Add("notes", it.notes);
                        this.Parametros.Add("flatRateAmount", it.flatRateAmount);
                        this.Parametros.Add("eventPerformedFrom", it.eventPerformedFrom);
                        this.Parametros.Add("chargeGlCode", it.chargeGlCode);
                        this.Parametros.Add("eventPerformedTo", it.eventPerformedTo);
                        this.Parametros.Add("gkey", it.gkey);
                        this.Parametros.Add("extractGkey", it.extractGkey);
                        this.Parametros.Add("extractClass", it.extractClass);
                        this.Parametros.Add("quantity", it.quantity);
                        this.Parametros.Add("quantityBilled", it.quantityBilled);
                        this.Parametros.Add("quantityUnit", it.quantityUnit);
                        this.Parametros.Add("rateBilled", it.rateBilled);
                        this.Parametros.Add("isFlatRate", it.isFlatRate);
                        this.Parametros.Add("totalTaxes", it.totalTaxes);
                        this.Parametros.Add("tariff_id", it.tariff_id);
                        this.Parametros.Add("tariff_amount", it.tariff_amount);
                        this.Parametros.Add("tariff_longDescription", it.tariff_longDescription);
                        this.Parametros.Add("created_user", this.created_user);
                       
                        result = BDOpe.ComandoInsertUpdateDeleteID(bcon, "[Bill].[nuevo_bil_expo_cab_invoice_charge]", this.Parametros);
                        if (!result.Exitoso)
                        {
                            return Respuesta.ResultadoOperacion<Int64>.CrearFalla(result.MensajeProblema);
                        }
                        it.id = result.Resultado;
                    }
           
                    scope.Complete();
#if DEBUG
                    this.LogEvent(created_user, this.actualMetodo, string.Format("Termina la transaccion {0}", DateTime.Now.ToString("dd/MM/yyyy HH:mm")));
#endif
                      return Respuesta.ResultadoOperacion<Int64 >.CrearResultadoExitoso(this.id.HasValue? this.id.Value:-1);

                }
                catch (Exception ex)
                {
                  var i=  this.LogError<Exception>(ex, this.actualMetodo, this.created_user);
                    return Respuesta.ResultadoOperacion<Int64>.CrearFalla(string.Format("Ha ocurrido la excepcion no.{0}, durante la transacción", i.HasValue ? i.Value : -1));
                }
            }
        }
    }
}
